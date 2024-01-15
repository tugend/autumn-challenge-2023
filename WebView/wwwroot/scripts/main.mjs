import StateClient from "./game-client.mjs";
import DomClient from "./dom-client.mjs";
import Controller from "./game-controller.mjs";
import UrlClient from "./url-client.mjs";

const main = async () => {
    // CONSTANTS
    const containerId = "conway";
    const namedInitialGame = "Blinker";
    const baseUrl = "/api/conway";

    // SETUP
    const settings = UrlClient.getSettings();
    const gameClient = new StateClient(baseUrl);
    const catalog = await gameClient.getCatalog();

    const initialSeed = settings.optionalSeedOverride || catalog.filter(x => x.key === namedInitialGame)[0];
    const initialState = {turn: settings.turn, grid: initialSeed.value};

    const controller = await new Controller(gameClient.fetchStates, settings.turnSpeedInMs, initialState);
    const domClient = new DomClient(initialSeed, catalog, settings.color);

    // Arrange dependencies
    controller.subscribe.toChanged(domClient.rerender);

    domClient.subscribe.toCellClick(controller.seed);
    domClient.subscribe.toResetBtnClick(controller.reset);
    domClient.subscribe.toTogglePlayBtnClick(controller.togglePause);
    domClient.subscribe.toCatalogSelect(catalogIndex => UrlClient.setSettings(domClient.getColor(), settings.turnSpeedInMs, catalog[catalogIndex]));

    // INITIAL RENDER
    domClient.renderTo(containerId);

    // START GAME LOOP
    console.debug("Initializing Conway's game of life", settings);
    await controller.start();
}

export default main;