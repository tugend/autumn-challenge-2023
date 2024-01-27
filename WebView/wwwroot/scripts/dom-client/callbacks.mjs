export default class Callbacks {
    /**
     * @type { () => void }
     */
    onTogglePlayBtnClick = () => { };

    /**
     * @type { (i: number, j: number) => void }
     */
    // eslint-disable-next-line no-unused-vars
    onCellClick = (_1, _2) => { };

    /**
     * @type { (catalogIndex: number) => Promise<void> }
     */
    // eslint-disable-next-line no-unused-vars
    onCatalogSelect = (_) => { };

    /**
     * @type { (theme: Theme) => {} }
     */
    // eslint-disable-next-line no-unused-vars
    onThemeSelect = (_) => { };

    /**
     * @type { () => void }
     */
    onResetBtnClick = () => { };
}