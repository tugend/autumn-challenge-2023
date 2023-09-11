window.conway = window.conway || {};

/**
 * @typedef {object} State
 * @property {number[][]} grid
 * @property {turn} number
 */

/**
 * @typedef SeedFunction
 * @param {number} o
 * @param {number} j
 * @returns { State }
 */

/**
 * @typedef CurrentFunction
 * @returns { State }
 */

/**
 * @typedef {object} Game
 * @property {function} init
 * @property {function} pause
 * @property {function} togglePause
 * @property {function} unpause
 * @property {SeedFunction} seed
 * @property {CurrentFunction} current
 */

/**
 * @typedef FetchStatesFunction
 * @param { State } seed
 */

/**
 * @typedef { object } BackendClient
 * @property { FetchStatesFunction } fetchStates
 */

/**
 * @typedef InitFunction
 * @param { string } containerId
 */

/**
 * @typedef {object} DomClient
 * @property { function } render
 * @property { InitFunction } init
 */

window.conway.controllerFactory = ((backendClientFactory, domClientFactory, gameFactory) => {
    /** @type {Game | null} */
    let game = null;

    /** @type {BackendClient | null} */
    let backendClient = null;

    /** @type {DomClient | null} */
    let domClient = null;

    /** @type {State | null} */
    let initialSeed = null;

    /**
     * @param { number } i
     * @param { number } j
     * @returns { Promise<void> }
     */
    const onCellClick = async (i, j) => {
        game.pause()
        
        const newSeed = await game.seed(i, j)
        const newStates = await backendClient.fetchStates(newSeed)
        newStates.forEach(x => x.turn = game.current().turn + x.turn);

        game = gameFactory(newStates, domClient.render)
        game.init();
    }

    /**
     * @returns { Promise<void> }
     */
    const onPauseBtnClick = () =>
        game.togglePause()

    /**
     * @returns { Promise<Game> }
     */
    const onResetBtnClick = async () => {
        game.pause()

        if (initialSeed == null) throw Error("Whoops. Seed was null!");
        const states = await backendClient.fetchStates(initialSeed);
        game = gameFactory(states, domClient.render)
        game.init();
        return game;
    }
    
    /**
     * @param { string } containerId
     * @param { State } seed
     * @returns { Game } game
     */
    const start = async (containerId, seed) => {
        initialSeed = seed;
        backendClient = backendClientFactory("/api/conway");
        domClient = domClientFactory(onCellClick, onPauseBtnClick, onResetBtnClick);
        
        domClient.init(containerId)
        const states = await backendClient.fetchStates(seed);
        game = gameFactory(states, domClient.render)
        game.init();
        game.unpause();
        return game;
    }
    
    return { start }
});

// TODO: cleanup    : Add/fix/update JSDOC
// TODO: tag        : version 1.0.0

// TODO: refactor   : replace function factories with classes?
// TODO: feature    : long running games by automated paging in slices of 1o states
// TODO: feature    : bigger grids!
// TODO: feature    : color coding of cell generations
// TODO: feature    : kill or birth illustration for mouse cell clicks
// TODO: add        : linting
// TODO: consider   : classes instead