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
     * @property { Theme } theme
     * @property { number } turnSpeedInMs
     * @property { number } turn
     * @property { (CatalogEntry|undefined) } optionalSeedOverride
     */
    static getSettings() {
        const urlParams = new URLSearchParams(location.search);
        const turnSpeedInMs = parseInt(urlParams.get("turn-speed") || "1000");

        const turn = parseInt(urlParams.get("turn") || "0");
        const theme = urlParams.get("theme") || "color";
        const optionalSeedOverride = tryParseSeed(urlParams.get("seed"));

        return {
            turnSpeedInMs,
            turn,
            theme,
            optionalSeedOverride
        };
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
     * @param {Theme} theme
     */
    static refreshWithNewTheme(theme) {
        const params = new URLSearchParams(location.search);
        params.set("theme", theme);
        window.location.search = params.toString();
    }
}