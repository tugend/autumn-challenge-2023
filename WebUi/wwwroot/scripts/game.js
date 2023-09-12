window.conway = window.conway || {};

window.conway.Game = class {
    
    /** @type { number } */
    #turnInMs = 1000;
    
    /** @type { number | null } */
    #timeoutId = null;

    /** @type { number } */
    #index = 0;
    
    #states;
    #onStateChange;
    
    constructor(states, onStateChange) {
        this.#states = states;
        this.#onStateChange = onStateChange;
    }
    
    /**
     * @param { Object } instance
     * @returns { Object } 
     */
    #deepCopy = (instance) =>
        JSON.parse(JSON.stringify(instance));
    
    isPaused = () =>
        this.#timeoutId == null;

    pause = () => {
        if (this.isPaused()) return;
        
        clearTimeout(this.#timeoutId)
        this.#timeoutId = null;
        this.#onStateChange(this.current(), this.isPaused())
    }

    nextTurn = () => {
        this.#timeoutId = setTimeout(() => {
            this.#index = Math.min(this.#states.length -1, this.#index + 1);
            this.#onStateChange(this.#states[this.#index], this.isPaused());
            this.nextTurn();
        }, this.#turnInMs);
    }

    unpause = () => {
        this.#timeoutId = setTimeout(this.nextTurn, this.#turnInMs);
        this.#onStateChange(this.current(), this.isPaused())
    }
    
    current = () =>
        this.#states[this.#index];
    
    /** 
     * @param { number } i
     * @param { number } j
     * @returns {Promise<State>} 
     */
    seed = async (i, j) => {
        let seededState = this.#deepCopy(this.current())
        seededState.grid[i][j] = seededState.grid[i][j] > 0 ? 0 : 1;
        seededState.turn = 0;
        return seededState;
    }
    
    togglePause = () =>
        this.isPaused()
            ? this.unpause()
            : this.pause();
    
    reset = () => {
        this.pause();
        this.#index = 0;
        this.#onStateChange(this.current(), this.isPaused())
    }
    
    init = () => 
        this.#onStateChange(this.current(), this.isPaused())
} 