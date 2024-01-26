import StateClient from "./engine-client.mjs";
import DomClient from "./dom-client.mjs";
import Controller from "./view-controller.mjs";
import UrlClient from "./url-client.mjs";

const main = async () => {
    window.conway = window.conway || { isMainLoopRunning: false };

    // CONSTANTS
    const containerId = "conway";
    const namedInitialGame = "Blinker";
    const baseUrl = "/api/conway";

    // SETUP
    const settings = UrlClient.getSettings();

    const engine = new StateClient(baseUrl);
    const catalog = await engine.getCatalog();

    const initialSeed = settings.optionalSeedOverride || catalog.filter(x => x.key === namedInitialGame)[0];
    const initialState = {turn: settings.turn, grid: initialSeed.value};

    const view = new DomClient(initialSeed, catalog, settings.theme);
    view.renderTo(containerId);
    view.subscribe.toCatalogSelect(catalogIndex => UrlClient.refreshWithNewSeed(catalog[catalogIndex]));
    view.subscribe.toThemeSelect(UrlClient.refreshWithNewTheme);

    const controller = await new Controller(engine.fetchStates, settings.turnSpeedInMs, initialState);
    controller.connectToDom(view);

    console.debug("Initializing Conway's game of life", settings);
    await controller.start();

    window.conway.isMainLoopRunning = true;
}

export default main;