/**
 * @typedef {object} State
 * @property {number} turn
 * @property {number[][]} grid
 *
 * @param {State} seed
 */
const initialSeed = {
    "turn": 1,
    "grid": [
        [1, 1, 0],
        [1, 0, 0],
        [0, 0, 0]
    ]
};

/**
 * @param { Response } response
 */
const asJson = response => response.json()

/**
 * @param { Object } content
 * @returns { RequestInit }
 */
const postRequest = (content) => ({ 
    method: "POST", 
    headers: {'Content-Type': 'application/json'}, 
    body: JSON.stringify(content) 
});

/**
 * @param { State } seed
 * @return { Promise<State[]> }
 */
const fetchStates = (seed) => 
    fetch("/api/conway", postRequest(seed)).then(asJson);

/**
 * @param { Number[][] } grid
 * @param {function(Number, Number, Number): string} map
 * @returns { String[] }
 */
const flatMap = (grid, map) => grid
    .map((row, i) => row.map((entry, j) => map(i, j, entry)))
    .flat();

/**
 * @function
 * @param { Number } i row index
 * @param { Number } j column index
 * @param { Number } value cell value
 * @return { String }
 */
const cellHtmlFactory = (i, j, value) => 
    `<div onclick="onCellClick(${i}, ${j})">${value}</div>`;

const renderContainerElm = (innerHtml) => document
    .querySelector("#state")
    .innerHTML = innerHtml;

const renderTurnCountElm = (turnCount) => document
    .querySelector("#turn > span")
    .innerText = turnCount;

const renderTogglePlayBtn = (buttonText) => document
    .getElementById('pause-btn')
    .innerText = buttonText;

async function startLoop() {
    let nextIndex = window.conway.index++;
    if (nextIndex >= window.conway.states.length) return;

    const state = window.conway.states[nextIndex];
    renderTurnCountElm(state.turn);
    renderContainerElm(flatMap(state.grid, cellHtmlFactory).join(""));

    unpause();
}

const unpause = () => 
    window.conway.timeoutId = setTimeout(startLoop, 2000);

const pause = ()=> {
    clearTimeout(conway.timeoutId)
    conway.timeoutId = null;
}

const isPaused = () =>
    window.conway.timeoutId == null

const togglePlay = () => {
    if (isPaused()) unpause();
    else pause();
}

const onTogglePause = () => {
    togglePlay()
    
    const buttonText = isPaused() ? "Continue" : "Pause";
    renderTogglePlayBtn(buttonText);
}

const onReset = async () => {
    window.conway.index = 0;
    pause();
    await startLoop();
}

const main = async () => {
    // TODO: enable paging (at 5 states left, try fetch run if more exists), 
    // TODO: enable pausing (stop fetching states, and stop play, click again -> continue)
    // TODO: enable seeding (prompt for new state given new seed, animate from there)
    // TODO: add reset button (clear all and toggle pause)
    const states = await fetchStates(initialSeed)
    window.conway = { index: 0, states, timeoutId: null, seed: initialSeed };
    await startLoop();
};

const onCellClick = async (i, j) => {
    if (!isPaused()) onTogglePause();

    let newSeed = { grid: window.conway.states[window.conway.index].grid } // TODO: do a proper copy here
    newSeed.grid[i][j] = 1; // TODO: this is a bit of a fragile mess and turn counter should not be reset!
    
    window.conway.index = 0;
    window.conway.seed = newSeed;
    window.conway.states = await fetchStates(newSeed);
    
    console.log('cell click!', window.conway.seed)

    const state = window.conway.states[window.conway.index]; // TODO: for some reason we get turn: 0 on replies!
    renderTurnCountElm(state.turn);
    renderContainerElm(flatMap(state.grid, cellHtmlFactory).join(""));
}

_ = main()