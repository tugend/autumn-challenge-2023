/**
 * @typedef {object} CatalogEntry
 * @property {string} key
 * @property { int[][] } value
 */

/**
 * @typedef {object} State
 * @property {number} turn
 * @property { int[][]} grid
 *
 * @param {State} seed
 */

/**
 * @typedef { object } DomSubscriptions
 * @property { (f: (i: number, j: number) => Promise<void>) => void } toCellClick
 * @property { (f: () => void) => void } toResetBtnClick
 * @property { (f: () => void) => void } toTogglePlayBtnClick
 * @property { (f: (catalogIndex: number) => Promise<void>) => void } toCatalogSelect
 */

/**
 * @typedef { object } DomClient 
 * @property { (containerId: string) => DomClient } renderTo
 * @property { (state: State, isPaused: boolean) => DomClient } rerender
 * @property { DomSubscriptions } subscribe
 */

/**
 * @typedef { object } GameSubscriptions
 * @property { (f: (current: State, isPaused: boolean) => void) => void } toChanged
 */

/**
 * @typedef {object} Game
 * @property { () => Promise<void> } start
 * @property { () => Game } unpause
 * @property { () => Game } pause
 * @property { () => void } togglePause
 * @property { () => void } reset
 * @property { GameSubscriptions } subscribe
 * @property { (states: State[]) => void } setState
 */