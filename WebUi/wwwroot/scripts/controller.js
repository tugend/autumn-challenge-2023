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
 * @typedef InitFunction
 * @param { string } containerId
 */

/**
 * @typedef {object} DomClient
 * @property { function } render
 * @property { InitFunction } init
 */

window.conway.Controller = class {

    /** @type {Game | null} */
    #game;

    /** @type {BackendClient} */
    #backendClient;

    /** @type {DomClient | null} */
    #domClient = null;

    /** @type {State | null} */
    #initialSeed = null;

    #gameFactory;
    
    #domClientFactory;
    
    constructor(backendClient, domClientFactory, gameFactory){
        this.#backendClient = backendClient;
        this.#domClientFactory = domClientFactory;
        this.#gameFactory = gameFactory;
    }

    /**
     * @param { string } containerId
     * @param { State } seed
     * @returns { Game } game
     */
    start = async (containerId, seed) => {
        this.#initialSeed = seed;
        this.#domClient = this.#domClientFactory(this.#onCellClick, this.#onPauseBtnClick, this.#onResetBtnClick);

        this.#domClient.init(containerId)
        const states = await this.#backendClient.fetchStates(seed);
        this.#game = this.#gameFactory(states, this.#domClient.render)
        this.#game.init();
        this.#game.unpause();
        return this.#game;
    }
    
    /**
     * @param { number } i
     * @param { number } j
     * @returns { Promise<void> }
     */
    #onCellClick = async (i, j) => {
        this.#game.pause()
        
        const newSeed = await this.#game.seed(i, j)
        const newStates = await this.#backendClient.fetchStates(newSeed)
        newStates.forEach(x => x.turn = this.#game.current().turn + x.turn);

        this.#game = this.#gameFactory(newStates, this.#domClient.render)
        this.#game.init();
    }

    /**
     * @returns { Promise<void> }
     */
    #onPauseBtnClick = () =>
        this.#game.togglePause()

    /**
     * @returns { Promise<void> }
     */
    #onResetBtnClick = async () => {
        this.#game.pause()

        if (this.#initialSeed == null) throw Error("Whoops. Seed was null!");
        const states = await this.#backendClient.fetchStates(this.#initialSeed);
        this.#game = this.#gameFactory(states, this.#domClient.render)
        this.#game.init();
    }
}

// TODO: fix        : memory leak (resources not disposed)
// TODO: refactor   : replace function factories with classes?
// TODO: feature    : long running games by automated paging in slices of 1o states
// TODO: feature    : bigger grids!
// TODO: feature    : color coding of cell generations
// TODO: feature    : kill or birth illustration for mouse cell clicks
// TODO: add        : linting
// TODO: consider   : classes instead