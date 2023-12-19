window.conway = window.conway || {};

window.conway.domClientFactory = (catalog) => {

    const nullHandler = () => {};
    let onCellClick = nullHandler;
    let onTogglePlayBtnClick = nullHandler;
    let onResetBtnClick = nullHandler
    let onCatalogSelect = nullHandler;

    const subscribeToCellClick = (f) => onCellClick = f;
    const subscribeToTogglePlayBtnClick = (f) => onTogglePlayBtnClick = f;
    const subscribeToResetBtnClick = (f) => onResetBtnClick = f;
    const subscribeToCatalogSelect = (f) => onCatalogSelect = f;
    
    const renderStateElm = (state, onClick) => {

        const cellHtmlFactory = (i, j, value) => {
            const elm = document.createElement('div')
            elm.innerText = `${value}`;
            elm.className = `life life-${Math.min(value, 9)}`;
            elm.onclick = () => onClick(i, j, value);
            return elm;
        }

        const children = state.grid
            .map((row, i) => row.map((value, j) => cellHtmlFactory(i, j, value)))
            .flat();
        
        document
            .querySelector("#state")
            .replaceChildren(...children);
    };

    /**
     * @param {number} turnCount
     */
    const renderTurnCountElm = (turnCount) => document
        .querySelector("#turn > span")
        .innerText = turnCount;

    const renderTogglePlayBtn = (isPaused) => {
        const elm = document.getElementById('pause-btn');
        elm.innerText = isPaused ? "Continue" : "Pause";
        elm.onclick = onTogglePlayBtnClick;
    };

    const renderResetBtn = () => {
        const elm = document.getElementById('reset-btn');
        elm.onclick = onResetBtnClick;
    };
    
    const createOption = (label, value) => {
        const elm = document.createElement("option");
        elm.value = value;
        elm.innerText = label;
        return elm;
    }
    
    const renderCatalog = (selector, catalog) => {
        const selectElm = document.querySelector(selector);
        
        selectElm.onchange = (event) => onCatalogSelect(event.target.value);
        
        Object
            .keys(catalog)
            .map(key => createOption(key, catalog[key]))
            .forEach(optionElm => selectElm.appendChild(optionElm));
    }

    const rerender = (state, isPaused) => {
        renderTogglePlayBtn(isPaused, onTogglePlayBtnClick);
        renderTurnCountElm(state.turn+1);
        renderStateElm(state, onCellClick);
        renderResetBtn();
        return that;
    }

    const renderTo = (containerId) => {
        document
            .getElementById(containerId)
            .innerHTML = `
                <h5 id="turn">Turn <span>1</span></h5>
                <div id="state" style="grid-template-columns: 1fr 1fr 1fr"></div>
                <br />
                <div id="controls">
                    <div id="pause-btn">Pause</div>
                    <div id="reset-btn">Reset</div>
                </div>
                <br />
                <aside id="input-catalog">
                    <select>
                        <option value="-1">---</option><!-- TODO: initialize with default! !>
                    </select>
                </aside>`;
        
        renderCatalog("#input-catalog > select", catalog);
        
        return that;
    }
    
    const that = { 
        renderTo, 
        rerender,
        subscribe: {
            toCellClick: subscribeToCellClick,
            toResetBtnClick: subscribeToResetBtnClick,
            toTogglePlayBtnClick: subscribeToTogglePlayBtnClick,
            toCatalogSelect: subscribeToCatalogSelect
        }};
    
    return that;
}