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