window.conway = window.conway || {};

window.conway.domClientFactory = (initialSeed, catalog) => {

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
        
        const stateElm = document.querySelector("#state");
        
        stateElm.className = `width-${state.grid[0].length}`;
        stateElm.replaceChildren(...children);
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
    
    const renderCatalog = (selector, catalog, selected) => {
        
        if (!catalog.some(x => x.key === selected.key))
        {
            catalog = [selected, ...catalog] 
        }
        
        const selectElm = document.querySelector(selector);
        
        catalog
            .map((entry, i) => createOption(entry.key,i))
            .forEach(optionElm => selectElm.appendChild(optionElm));

        selectElm.selectedIndex = catalog.map(x => x.key).indexOf(selected.key); // TODO: do better
        selectElm.onchange = (event) => onCatalogSelect(event.target.value);
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
                <div id="state"></div>
                <br />
                <div id="controls">
                    <div id="pause-btn">Pause</div>
                    <div id="reset-btn">Reset</div>
                </div>
                <br />
                <aside id="input-catalog">
                    <select></select>
                </aside>`;
        
        renderCatalog("#input-catalog > select", catalog, initialSeed);
        
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