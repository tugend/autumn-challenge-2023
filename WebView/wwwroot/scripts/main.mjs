import StateClient from "./engine-client.mjs";
import DomClient from "./dom-client.mjs";
import Controller from "./view-controller.mjs";
import UrlClient from "./url-client.mjs";

const main = async () => {

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

    const view = new DomClient(initialSeed, catalog, settings.color);
    view.renderTo(containerId);
    view.subscribe.toCatalogSelect(catalogIndex => UrlClient.refreshWithNewSeed(catalog[catalogIndex]));
    view.subscribe.toColorSelect(color => UrlClient.refreshWithNewColor(color));

    const controller = await new Controller(engine.fetchStates, settings.turnSpeedInMs, initialState);
    controller.connectToDom(view);

    console.debug("Initializing Conway's game of life", settings);
    await controller.start();
}

export default main;