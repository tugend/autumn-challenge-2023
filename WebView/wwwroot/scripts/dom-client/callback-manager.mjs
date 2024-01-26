export default class CallbackManager {
    /**
     * @param {Callbacks} callbacks
     */
    #callbacks;

    constructor(callbacks) {
        this.#callbacks = callbacks;
    }

    /**
     * @type { (f: () => void) => {} }
     */
    toTogglePlayBtnClick = (f) => this.#callbacks.onTogglePlayBtnClick = f;

    /**
     * @type { (f: (i: number, j: number) => void) => {} }
     */
    toCellClick = (f) => this.#callbacks.onCellClick = f;

    /**
     * @type { (f: (catalogIndex: number) => {}) => {} }
     */
    toCatalogSelect = (f) => this.#callbacks.onCatalogSelect = f;

    /**
     * @type { (f: (theme: Theme) => {}) => {} }
     */
    toThemeSelect = (f) => this.#callbacks.onThemeSelect = f;

    /**
     * @type { (f: () => void) => {} }
     */
    toResetBtnClick = (f) => this.#callbacks.onResetBtnClick = f;
}