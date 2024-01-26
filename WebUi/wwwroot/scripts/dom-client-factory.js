window.conway = window.conway || {};

/**
 * @param {CatalogEntry} initialSeed
 * @param {CatalogEntry[]} catalog
 * @param {"binary"|"color"} color
 * @returns {DomClient}
 */
window.conway.domClientFactory = (initialSeed, catalog, color) => {

    const nullHandler = () => {};

    let onCellClick = nullHandler;
    let onTogglePlayBtnClick = nullHandler;
    let onResetBtnClick = nullHandler;
    let onCatalogSelect = nullHandler;

    const subscribeToCellClick = (f) => onCellClick = f;
    const subscribeToTogglePlayBtnClick = (f) => onTogglePlayBtnClick = f;
    const subscribeToResetBtnClick = (f) => onResetBtnClick = f;
    const subscribeToCatalogSelect = (f) => onCatalogSelect = f;

    /**
     * @param { State } state
     * @param { (i: number, j: number, value: string) => void } onClick
     */
    const renderStateElm = (state, onClick) => {

        /**
         * @param { number } i
         * @param { number } j
         * @param { number } value
         * @returns {HTMLDivElement}
         */
        const cellHtmlFactory = (i, j, value) => {
            const elm = document.createElement("div");
            elm.innerText = `${value}`;
            elm.className = `life life-${Math.min(value, 9)}`;
            elm.onclick = () => onClick(i, j, value + "");
            return elm;
        };

        const children = state.grid
            .map((row, i) => row.map((value, j) => cellHtmlFactory(i, j, value)))
            .flat();

        const stateElm = document.querySelector("#state");

        stateElm.style.gridTemplateColumns = [...Array(state.grid[0].length)]
            .map(() => "1fr")
            .join(" ");

        stateElm.replaceChildren(...children);
    };

    /**
     * @param {number} turnCount
     */
    const renderTurnCountElm = (turnCount) => document
        .querySelector("#turn > span")
        .innerText = turnCount;

    /**
     * @param { boolean } isPaused
     */
    const renderTogglePlayBtn = (isPaused) => {
        const elm = document.getElementById("pause-btn");
        elm.innerText = isPaused ? "Continue" : "Pause";
        elm.onclick = onTogglePlayBtnClick;
    };

    const renderResetBtn = () => {
        const elm = document.getElementById("reset-btn");
        elm.onclick = onResetBtnClick;
    };

    const getColor = () =>
        document.getElementById("state").className;

    const renderColorBtn = () => {
        const elm = document.getElementById("color-btn");
        elm.onclick = () => {
            const stateElm = document.getElementById("state");
            const color = getColor();
            stateElm.className = color === "binary" ? "color" : "binary";
        };
    };

    /**
     * @param { string } label
     * @param { number } value
     * @returns {HTMLOptionElement}
     */
    const createOption = (label, value) => {
        const elm = document.createElement("option");
        elm.value = value + "";
        elm.innerText = label;
        return elm;
    };

    /**
     * @param { string } selector
     * @param { CatalogEntry[] } catalog
     * @param { CatalogEntry } selected
     */
    const renderCatalog = (selector, catalog, selected) => {
        
        if (!catalog.some(x => x.key === selected.key))
        {
            catalog = [selected, ...catalog];
        }
        
        const selectElm = document.querySelector(selector);
        
        catalog
            .map((entry, i) => createOption(entry.key, i))
            .forEach(optionElm => selectElm.appendChild(optionElm));

        selectElm.selectedIndex = catalog.map(x => x.key).indexOf(selected.key);
        selectElm.onchange = (event) => onCatalogSelect(event.target.value);
    };

    /**
     * @param { State } state
     * @param { boolean } isPaused
     * @returns { DomClient }
     */
    const rerender = (state, isPaused) => {
        renderTogglePlayBtn(isPaused);
        renderTurnCountElm(state.turn+1);
        renderStateElm(state, onCellClick);
        renderResetBtn();
        return that;
    };

    /**
     * @param { string } containerId
     * @returns { DomClient }
     */
    const renderTo = (containerId) => {
        document
            .getElementById(containerId)
            .innerHTML = `
                <h5 id="turn">Turn <span>1</span></h5>
                <div id="state" class="${color}"></div>
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
        
        renderCatalog("#input-catalog > select", catalog, initialSeed);
        renderColorBtn();
        
        return that;
    };

    /**
     * @type {DomClient}
     */
    const that = { 
        renderTo, 
        rerender,
        getColor,
        subscribe: {
            toCellClick: subscribeToCellClick,
            toResetBtnClick: subscribeToResetBtnClick,
            toTogglePlayBtnClick: subscribeToTogglePlayBtnClick,
            toCatalogSelect: subscribeToCatalogSelect
        }};
    
    return that;
};