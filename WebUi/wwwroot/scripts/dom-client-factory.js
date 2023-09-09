window.conway = window.conway || {};

window.conway.domClientFactory = (onCellClick, onTogglePlayBtnClick, onResetBtnClick) => {
    /**
     * @param { Number[][] } grid
     * @param {function(Number, Number, Number): T} map
     * @returns { T[] }
     */
    const flatMap = (grid, map) => grid
        .map((row, i) => row.map((entry, j) => map(i, j, entry)))
        .flat();
    
    const renderStateElm = (state, onClick) => {

        const cellHtmlFactory = (i, j, value) => {
            const elm = document.createElement('div')
            elm.innerText = `${value}`;
            elm.className = `life life-${Math.min(value, 9)}`;
            elm.onclick = () => onClick(i, j, value);
            return elm;
        }

        const children = flatMap(state.grid, cellHtmlFactory)
        
        document
            .querySelector("#state")
            .replaceChildren(...children); // TODO: figure out how to get my line breaks back! :/
    };

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
    
    const render = (state, isPaused) => {
        renderTogglePlayBtn(isPaused, onTogglePlayBtnClick);
        renderTurnCountElm(state.turn+1);
        renderStateElm(state, onCellClick);
        renderResetBtn();
    }
    
    const init = (containerId) => {
        document
            .getElementById(containerId)
            .innerHTML = `
                <h5 id="turn">Turn <span>1</span></h5>
                <div id="state"></div>
                <br />
                <div id="controls">
                    <div id="pause-btn">Pause</div>
                    <div id="reset-btn">Reset</div>
                </div>`;
    }
    
    return { init, render };
}