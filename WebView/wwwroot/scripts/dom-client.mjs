import Callbacks from "./dom-client/callbacks.mjs";
import CallbackManager from "./dom-client/callback-manager.mjs";

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
    elm.className = `cell life-${Math.min(value, 9)}`;
    elm.onclick = () => onClick(i, j);
    return elm;
};

export default class DomClient {
    /**
     * @type {CatalogEntry}
     */
    #initialSeed

    /**
     * @type {CatalogEntry[]}
     */
    #catalog

    /**
     * @type {Theme} theme
     */
    #theme;

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
     * @param {Theme} theme
     */
    constructor(initialSeed, catalog, theme) {
        this.#initialSeed = initialSeed;
        this.#catalog = catalog;
        this.#theme = theme;
        this.#subscriptions = new Callbacks();
        this.subscribe = new CallbackManager(this.#subscriptions);
    }

    /**
     * @param { State } state
     * @param { boolean } isPaused
     * @returns { DomClient }
     */
    rerender = (state, isPaused) => {
        this.#renderTogglePlayBtn(isPaused);
        this.#renderTurnCountElm(state.turn+1);
        this.#renderStateElm(state, this.#subscriptions.onCellClick);
        this.#renderResetBtn();
    }

    /**
     * @param { string } containerId
     * @returns { DomClient }
     */
    renderTo = (containerId) => {
        document
            .getElementById(containerId)
            .innerHTML = `
                <h2 id="turn">Turn <span>1</span></h2>
                <div id="state" class="theme-${this.#theme}"></div>
                <br />
                <div id="controls">
                    <div id="pause-btn" class="btn">Pause</div>
                    <div id="reset-btn" class="btn">Reset</div>
                    <div id="theme-btn" class="btn">Theme</div>
                </div>
                <br />
                <aside id="seed-catalog">
                    <select></select>
                </aside>`;

        this.#renderCatalog("#seed-catalog > select", this.#catalog, this.#initialSeed);
        this.#renderThemeBtn();
    }

    /**
     * @param { State } state
     * @param { (i: number, j: number) => void } onClick
     */
    #renderStateElm = (state, onClick) => {
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
    #renderTurnCountElm = (turnCount) => {
        document
            .querySelector("#turn > span")
            .innerText = turnCount;
    }

    /**
     * @param { boolean } isPaused
     */
    #renderTogglePlayBtn = (isPaused) => {
        const elm = document.getElementById("pause-btn");
        elm.innerText = isPaused ? "Continue" : "Pause";
        elm.onclick = this.#subscriptions.onTogglePlayBtnClick;
    }

    #renderResetBtn = () => {
        const elm = document.getElementById("reset-btn");
        elm.onclick = this.#subscriptions.onResetBtnClick;
    }

    /**
     * @returns { Theme }
     */
    #getTheme = () => {
        // NOTE: this is a bit of a simple hack where we store a bit of state in the DOM.
        // A better way may be to either use the DOM data attributes, or likely best and most consistent, keep an internal state instead.
        return document.getElementById("state").className.replace("theme-", "");
    }

    #renderThemeBtn = () => {
        var elm = document.getElementById("theme-btn")

        elm.onclick = () => {
            const currentTheme = this.#getTheme();
            const newTheme = currentTheme === "binary" ? "color" : "binary"
            this.#subscriptions.onThemeSelect(newTheme)
        }
    }

    /**
     * @param { string } label
     * @param { number } value
     * @returns {HTMLOptionElement}
     */
    #createOption = (label, value) => {
        const elm = document.createElement("option");
        elm.value = value + "";
        elm.innerText = label;
        return elmn
    }

    /**
     * @param { string } selector
     * @param { CatalogEntry[] } catalog
     * @param { CatalogEntry } selected
     */
    #renderCatalog = (selector, catalog, selected)  => {

        if (!catalog.some(x => x.key === selected.key))
        {
            catalog = [selected, ...catalog];
        }

        const selectElm = document.querySelector(selector);

        catalog
            .map((entry, i) => this.#createOption(entry.key, i))
            .forEach(optionElm => selectElm.appendChild(optionElm));

        selectElm.selectedIndex = catalog.map(x => x.key).indexOf(selected.key);
        selectElm.onchange = (event) => this.#subscriptions.onCatalogSelect(event.target.value);
    }
}