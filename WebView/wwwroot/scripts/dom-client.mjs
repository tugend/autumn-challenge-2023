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

}

// TODO: separate into a manager that allow us to set callbacks
// and an internal object that allow us to call the callbacks.
// This should avoid shared state where others can call the callbacks directly.
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

    subscriptions = new Callbacks();

    /**
     * @param {CatalogEntry} initialSeed
     * @param {CatalogEntry[]} catalog
     * @param {"binary"|"color"} color
     */
    constructor(initialSeed, catalog, color) {
        this.#initialSeed = initialSeed;
        this.#catalog = catalog;
        this.#color = color;
    }

    /**
     * @param { State } state
     * @param { (i: number, j: number) => void } onClick
     */
    renderStateElm(state, onClick) {
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
    renderTurnCountElm(turnCount) {
        document
            .querySelector("#turn > span")
            .innerText = turnCount;
    }

    /**
     * @param { boolean } isPaused
     */
    renderTogglePlayBtn(isPaused) {
        const elm = document.getElementById("pause-btn");
        elm.innerText = isPaused ? "Continue" : "Pause";
        elm.onclick = this.subscriptions.onTogglePlayBtnClick;
    }

    renderResetBtn() {
        const elm = document.getElementById("reset-btn");
        elm.onclick = this.subscriptions.onResetBtnClick;
    }

    getColor() {
        return document.getElementById("state").className;
    }

    renderColorBtn() {
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
    createOption(label, value) {
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
    renderCatalog(selector, catalog, selected)  {

        if (!catalog.some(x => x.key === selected.key))
        {
            catalog = [selected, ...catalog];
        }

        const selectElm = document.querySelector(selector);

        catalog
            .map((entry, i) => this.createOption(entry.key, i))
            .forEach(optionElm => selectElm.appendChild(optionElm));

        selectElm.selectedIndex = catalog.map(x => x.key).indexOf(selected.key);
        selectElm.onchange = (event) => this.subscriptions.onCatalogSelect(event.target.value);
    }

    /**
     * @param { State } state
     * @param { boolean } isPaused
     * @returns { DomClient }
     */
    rerender(state, isPaused)  {
        this.renderTogglePlayBtn(isPaused);
        this.renderTurnCountElm(state.turn+1);
        this.renderStateElm(state, this.subscriptions.onCellClick);
        this.renderResetBtn();
        return this;
    }

    /**
     * @param { string } containerId
     * @returns { DomClient }
     */
    renderTo (containerId) {
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

        return this;
    }
}