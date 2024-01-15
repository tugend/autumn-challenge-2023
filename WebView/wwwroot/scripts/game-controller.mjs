const deepCopy = (instance) => {
    return JSON.parse(JSON.stringify(instance));
}

class CallbackManager {
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

class Callbacks {
    /**
     * @type { (current: State, isPaused: boolean) => void }
     */
    onChanged = (_1, _2) => {};
}

export default class GameController {
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
    #initialStates = [];

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
    subscribe;

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
        this.subscribe = new CallbackManager(this.#callbacks);
    }

    start = async () => {
        const states = await this.#fetchStates(this.#start);
        this.#initialStates = states;
        this.#currentStates = states;
        this.unpause();
    }

    setState = (_states) => {
        this.#currentStates = _states.map(x => {
            x.turn = this.#currentStates[this.#index].turn + x.turn;
            return x;
        });
        this.#index = 0;
        this.reportChange();
    };

    #isPaused = () =>
        this.#timeoutId == null;

    pause = () => {
        if (this.#isPaused()) return;

        clearTimeout(this.#timeoutId);
        this.#timeoutId = null;
        this.reportChange();
    };

    #nextTurn = async () => {
        console.debug("next turn", this.#index);

        if (this.#index === this.#currentStates.length - 1) {
            const nextPage = await this.#fetchStates(this.#currentStates[this.#index]);
            this.#currentStates = [...this.#currentStates.slice(0, this.#currentStates.length - 1), ...nextPage];
        }

        this.#timeoutId = setTimeout(() => {
            this.#index = Math.min(this.#currentStates.length - 1, this.#index + 1);
            this.reportChange();
            this.#nextTurn();
        }, this.#turnInMs);
    };

    unpause = () => {
        this.#timeoutId = setTimeout(this.#nextTurn, this.#turnInMs);
        this.reportChange();
    };

    #current = () =>
        this.#currentStates[this.#index];

    seed = async (i, j) => {
        const seededState = deepCopy(this.#current());

        const entry = seededState.grid[i][j];
        seededState.grid[i][j] = entry > 0 ? 0 : 1;
        seededState.turn = 0;

        return seededState;
    };

    togglePause = () =>
        this.#isPaused()
            ? this.unpause()
            : this.pause();

    reset = () => {
        this.#currentStates = this.#initialStates;
        this.#index = 0;
        this.reportChange();
    };

    // TODO: naming---
    reportChange = () => {
        console.debug("report change")
        this.#callbacks.onChanged(this.#current(), this.#isPaused());
    };
}