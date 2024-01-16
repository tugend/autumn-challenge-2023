/**
 * @param { string | undefined } seedOrUndefined
 * @returns { CatalogEntry | undefined }
 */
const tryParseSeed = (seedOrUndefined) => seedOrUndefined === undefined
    ? undefined
    : /** @type CatalogEntry */ JSON.parse(decodeURIComponent(seedOrUndefined));

export default class UrlClient {
    /**
     * @typedef { object } Settings
     * @property { 'color'|'binary' } color
     * @property { number } turnSpeedInMs
     * @property { number } turn
     * @property { (CatalogEntry|undefined) } optionalSeedOverride
     */
    static getSettings() {
        const urlParams = new URLSearchParams(location.search);
        const turnSpeedInMs = parseInt(urlParams.get("turn-speed") || "1000");

        const turn = parseInt(urlParams.get("turn") || "0");
        const color = urlParams.get("color") || "binary";
        const optionalSeedOverride = tryParseSeed(urlParams.get("seed"));

        return {
            turnSpeedInMs,
            turn,
            color,
            optionalSeedOverride
        }
    }

    /**
     * @param {CatalogEntry} seed
     */
    static refreshWithNewSeed(seed) {
        const params = new URLSearchParams(location.search);
        params.set("seed", encodeURIComponent(JSON.stringify(seed)));
        window.location.search = params.toString();
    }

    /**
     * @param {'color'|'binary'} color
     */
    static refreshWithNewColor(color) {
        const params = new URLSearchParams(location.search);
        params.set("color", color);
        window.location.search = params.toString();
    }
}