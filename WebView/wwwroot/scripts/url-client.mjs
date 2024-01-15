/**
 * @param { string | undefined } seedOrUndefined
 * @returns { CatalogEntry | undefined }
 */
const tryParseSeed = (seedOrUndefined) => seedOrUndefined === undefined
    ? undefined
    : /** @type CatalogEntry */ JSON.parse(decodeURIComponent(seedOrUndefined));

export default class UrlClient {

    /**
     * @returns {Settings}
     */
    getSettings() {
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
     * @param {Settings} settings
     */
     setSettings(settings) {
        const params = new URLSearchParams(location.search);
        params.set("color", settings.color);
        params.set("turn-speed", settings.turnSpeedInMs + "");
        params.set("seed", encodeURIComponent(JSON.stringify(settings.optionalSeedOverride)));

        window.location.search = params.toString();
    }
}