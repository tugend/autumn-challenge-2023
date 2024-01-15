import GameClient from "./game-client.mjs";
import DomClient from "./dom-client.mjs";
import GameController from "./game-controller.mjs";
import UrlClient from "./url-client.mjs";
import arrangeDependencies from "./arrange-dependencies.mjs";

const main = async () => {
    // CONSTANTS
    const containerId = "conway";
    const namedInitialGame = "Blinker";
    const baseUrl = "/api/conway";

    // SETUP
    const urlClient = new UrlClient();
    const settings = urlClient.getSettings();
    const gameClient = new GameClient(baseUrl);
    const catalog = await gameClient.getCatalog();

    const initialSeed = settings.optionalSeedOverride || catalog.filter(x => x.key === namedInitialGame)[0];
    const initialState = {turn: settings.turn, grid: initialSeed.value};

    const controller = await new GameController(gameClient.fetchStates, settings.turnSpeedInMs, initialState);
    const domClient = new DomClient(initialSeed, catalog, settings.color);
    arrangeDependencies(settings, urlClient, domClient, controller, gameClient, catalog); // TODO: ???

    // INITIAL RENDER
    domClient.renderTo(containerId);

    // START GAME LOOP
    console.debug("Initializing Conway's game of life", settings);
    await controller.start();
}

export default main;