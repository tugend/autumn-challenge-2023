window.conway = window.conway || {};

window.conway.domClient = (() => {
    const that = {};

    /**
     * @param { Number[][] } grid
     * @param {function(Number, Number, Number): string} map
     * @returns { String[] }
     */
    const flatMap = (grid, map) => grid
        .map((row, i) => row.map((entry, j) => map(i, j, entry)))
        .flat();
    
    that.renderContainerElm = (innerHtml) => document
        .querySelector("#state")
        .innerHTML = innerHtml;

    that.renderTurnCountElm = (turnCount) => document
        .querySelector("#turn > span")
        .innerText = turnCount;

    that.renderTogglePlayBtn = (isPaused) => document
        .getElementById('pause-btn')
        .innerText = isPaused ? "Continue" : "Pause";

    /**
     * @function
     * @param { Number } i row index
     * @param { Number } j column index
     * @param { Number } value cell value
     * @return { String }
     */
    that.cellHtmlFactory = (i, j, value) =>
        `<div onclick="onCellClick(${i}, ${j})">${value}</div>`;

    that.render = (state, isPaused) => {
        console.log('render!', state, isPaused)
        that.renderTogglePlayBtn(isPaused);
        that.renderTurnCountElm(state.turn+1);
        that.renderContainerElm(flatMap(state.grid, that.cellHtmlFactory).join(""));
    }
    
    return that;
})();