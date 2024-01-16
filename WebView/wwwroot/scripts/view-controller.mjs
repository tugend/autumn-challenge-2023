const deepCopy = (instance) => {
    return JSON.parse(JSON.stringify(instance));
}

export class Callbacks {
    /**
     * @type { (current: State, isPaused: boolean) => void }
     */
    onChanged = (_1, _2) => {
    };
}

export class CallbackManager {
    /**
     * @type {Callbacks}
     */
    #callbacks;

    constructor(callbacks) {
        this.#callbacks = callbacks;
    }

    /**
     * @type { (f: (current: State, isPaused: boolean) => void) => {} }
     */
    toChanged = (f) => this.#callbacks.onChanged = f;
}

export default class ViewController {
    /**
     * @type {number}
     */
    #turnInMs;

    /**
     * @type {number | null}
     */
    #timeoutId;

    /**
     * @type number
     */
    #index;

    /**
     * @type { State }
     */
    #start;

    /**
     * @type {State[]}
     */
    #startStates = [];

    /**
     * @type {State[]}
     */
    #currentStates = [];

    /**
     * @type {(State) => Promise<State[]>}
     */
    #fetchStates;

    /**
     * @type {Callbacks}
     */
    #callbacks;

    /**
     * @type {CallbackManager}
     */
    #subscribe;

    /**
     * @param { (State) => Promise<State[]> } fetchStates
     * @param { Number } turnSpeedInMs
     * @param { State } start
     */
    constructor(fetchStates, turnSpeedInMs, start) {
        this.#turnInMs = turnSpeedInMs;
        this.#timeoutId = null;
        this.#index = 0;
        this.#start = start;
        this.#fetchStates = fetchStates;
        this.#callbacks = new Callbacks();
        this.#subscribe = new CallbackManager(this.#callbacks);
    }

    connectToDom = (domClient) => {
        this.#subscribe.toChanged(domClient.rerender);

        domClient.subscribe.toCellClick(this.seed);
        domClient.subscribe.toResetBtnClick(this.resetToStart);
        domClient.subscribe.toTogglePlayBtnClick(this.togglePause);
    }

    start = async () => {
        const states = await this.#fetchStates(this.#start);
        this.#startStates = states;
        this.#currentStates = states;
        this.#unpause();
    }


    seed = async (i, j) => {
        this.#pause();

        const newState = deepCopy(this.#current());
        const entry = newState.grid[i][j];
        newState.grid[i][j] = entry > 0 ? 0 : 1;
        newState.turn = 0;

        const newStates = await this.#fetchStates(newState);
        this.#setState(newStates);
    };

    togglePause = () =>
        this.#isPaused()
            ? this.#unpause()
            : this.#pause();

    resetToStart = () => {
        this.#currentStates = this.#startStates;
        this.#index = 0;
        this.#pause();
        this.#reportChange();
    };

    #setState = (_states) => {
        this.#currentStates = _states.map(x => ({ ...x, turn:  this.#currentStates[this.#index].turn + x.turn }));
        this.#index = 0;
        this.#reportChange();
    };

    #isPaused = () =>
        this.#timeoutId == null;

    #pause = () => {
        if (this.#isPaused()) return;

        clearTimeout(this.#timeoutId);
        this.#timeoutId = null;
        this.#reportChange();
    };

    #nextTurn = async () => {
        console.debug("next turn", this.#index);

        if (this.#index === this.#currentStates.length - 1) {
            const nextPage = await this.#fetchStates(this.#currentStates[this.#index]);
            this.#currentStates = [...this.#currentStates.slice(0, this.#currentStates.length - 1), ...nextPage];
        }

        this.#timeoutId = setTimeout(() => {
            this.#index = Math.min(this.#currentStates.length - 1, this.#index + 1);
            this.#reportChange();
            this.#nextTurn();
        }, this.#turnInMs);
    };

    #unpause = () => {
        if (!this.#isPaused()) return;

        this.#timeoutId = setTimeout(this.#nextTurn, this.#turnInMs);
        this.#reportChange();
    };

    #current = () =>
        this.#currentStates[this.#index];

    #reportChange = () => {
        this.#callbacks.onChanged(this.#current(), this.#isPaused());
    };
}