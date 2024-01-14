/**
 * @param { number } i
 * @param { number } j
 * @param { number } value
 * @param { (i: number, j: number) => void } onClick
 * @returns {HTMLDivElement}
 */
const createLifeCellElm = (i, j, value, onClick) => {
    const elm = document.createElement("div");
    elm.innerText = `${value}`;
    elm.className = `life life-${Math.min(value, 9)}`;
    elm.onclick = () => onClick(i, j);
    return elm;
};

class CallbackManager {
    /**
     * @param {Callbacks} callbacks
     */
    constructor(callbacks) {
    }

    /**
     * @type { (f: () => void) => void }
     */
    toTogglePlayBtnClick = (_) => {};

    /**
     * @type { (f: (i: number, j: number) => void) => void }
     */
        // eslint-disable-next-line no-unused-vars
    toCellClick = (_) => {};

    /**
     * @type { (f: (catalogIndex: number) => Promise<void>) => void }
     */
        // eslint-disable-next-line no-unused-vars
    toCatalogSelect = (_) => {};

    /**
     * @type { (f: () => void) => void }
     */
    toResetBtnClick = (_) => {};
}

class Callbacks {
    /**
     * @type { () => void }
     */
    onTogglePlayBtnClick = () => {};

    /**
     * @type { (i: number, j: number) => void }
     */
    // eslint-disable-next-line no-unused-vars
    onCellClick = (_1, _2) => {};

    /**
     * @type { (catalogIndex: number) => Promise<void> }
     */
    // eslint-disable-next-line no-unused-vars
    onCatalogSelect = (_) => {};

    /**
     * @type { () => void }
     */
    onResetBtnClick = () => {};
}

export default class DomClient {
    /**
     * @type {CatalogEntry}
     */
    #initialSeed;

    /**
     * @type {CatalogEntry[]}
     */
    #catalog;

    /**
     * @type {"binary"|"color"} color
     */
    #color;

    /**
     * @type { Callbacks }
     */
    #subscriptions;

    /**
     * @type { CallbackManager }
     */
    subscribe

    /**
     * @param {CatalogEntry} initialSeed
     * @param {CatalogEntry[]} catalog
     * @param {"binary"|"color"} color
     */
    constructor(initialSeed, catalog, color) {
        this.#initialSeed = initialSeed;
        this.#catalog = catalog;
        this.#color = color;
        this.#subscriptions = new Callbacks();
        this.subscribe = new CallbackManager(this.#subscriptions);
    }

    /**
     * @param { State } state
     * @param { (i: number, j: number) => void } onClick
     */
    renderStateElm = (state, onClick) => {
        const children = state
            .grid.map((row, i) =>
                row.map((value, j) =>
                    createLifeCellElm(i, j, value, onClick))
            )
            .flat();

        const stateElm = document.querySelector("#state");

        stateElm.style.gridTemplateColumns = [...Array(state.grid[0].length)]
            .map(() => "1fr")
            .join(" ");

        stateElm.replaceChildren(...children);
    }

    /**
     * @param {number} turnCount
     */
    renderTurnCountElm = (turnCount) => {
        document
            .querySelector("#turn > span")
            .innerText = turnCount;
    }

    /**
     * @param { boolean } isPaused
     */
    renderTogglePlayBtn = (isPaused) => {
        const elm = document.getElementById("pause-btn");
        elm.innerText = isPaused ? "Continue" : "Pause";
        elm.onclick = this.#subscriptions.onTogglePlayBtnClick;
    }

    renderResetBtn = () => {
        const elm = document.getElementById("reset-btn");
        elm.onclick = this.#subscriptions.onResetBtnClick;
    }

    getColor = () => {
        return document.getElementById("state").className;
    }

    renderColorBtn = () => {
        const elm = document.getElementById("color-btn");
        elm.onclick = () => {
            const stateElm = document.getElementById("state");
            const color = this.getColor();
            stateElm.className = color === "binary" ? "color" : "binary";
        };
    }

    /**
     * @param { string } label
     * @param { number } value
     * @returns {HTMLOptionElement}
     */
    createOption = (label, value) => {
        const elm = document.createElement("option");
        elm.value = value + "";
        elm.innerText = label;
        return elm;
    }

    /**
     * @param { string } selector
     * @param { CatalogEntry[] } catalog
     * @param { CatalogEntry } selected
     */
    renderCatalog = (selector, catalog, selected)  => {

        if (!catalog.some(x => x.key === selected.key))
        {
            catalog = [selected, ...catalog];
        }

        const selectElm = document.querySelector(selector);

        catalog
            .map((entry, i) => this.createOption(entry.key, i))
            .forEach(optionElm => selectElm.appendChild(optionElm));

        selectElm.selectedIndex = catalog.map(x => x.key).indexOf(selected.key);
        selectElm.onchange = (event) => this.#subscriptions.onCatalogSelect(event.target.value);
    }

    /**
     * @param { State } state
     * @param { boolean } isPaused
     * @returns { DomClient }
     */
    rerender = (state, isPaused) => {
        this.renderTogglePlayBtn(isPaused);
        this.renderTurnCountElm(state.turn+1);
        this.renderStateElm(state, this.#subscriptions.onCellClick);
        this.renderResetBtn();
    }

    /**
     * @param { string } containerId
     * @returns { DomClient }
     */
    renderTo = (containerId) => {
        document
            .getElementById(containerId)
            .innerHTML = `
                <h5 id="turn">Turn <span>1</span></h5>
                <div id="state" class="${this.#color}"></div>
                <br />
                <div id="controls">
                    <div id="pause-btn">Pause</div>
                    <div id="reset-btn">Reset</div>
                    <div id="color-btn">Color</div>
                </div>
                <br />
                <aside id="input-catalog">
                    <select></select>
                </aside>`;

        this.renderCatalog("#input-catalog > select", this.#catalog, this.#initialSeed);
        this.renderColorBtn();
    }
}