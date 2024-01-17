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