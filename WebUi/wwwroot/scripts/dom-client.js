window.conway = window.conway || {};

window.conway.domClient = (() => {
    /**
     * @param { Number[][] } grid
     * @param {function(Number, Number, Number): string} map
     * @returns { String[] }
     */
    const flatMap = (grid, map) => grid
        .map((row, i) => row.map((entry, j) => map(i, j, entry)))
        .flat();
    
    const renderContainerElm = (innerHtml) => document
        .querySelector("#state")
        .innerHTML = innerHtml;

    const renderTurnCountElm = (turnCount) => document
        .querySelector("#turn > span")
        .innerText = turnCount;

    const renderTogglePlayBtn = (isPaused) => document
        .getElementById('pause-btn')
        .innerText = isPaused ? "Continue" : "Pause";

    /**
     * @function
     * @param { Number } i row index
     * @param { Number } j column index
     * @param { Number } value cell value
     * @return { String }
     */
    // TODO: use events and a click handler instead or somethign like that...
    // I want to inject an async handler in the dom client
    const cellHtmlFactory = (i, j, value) =>
        `<div 
            onclick="window.conway.controllers.onCellClick(${i}, ${j}, ${value})"
            class="life life-${ Math.min(value, 10) }">
            ${value}
        </div>`;

    const render = (state, isPaused) => {
        renderTogglePlayBtn(isPaused);
        renderTurnCountElm(state.turn+1);
        renderContainerElm(flatMap(state.grid, cellHtmlFactory).join(""));
    }
    
    return { render };
})();