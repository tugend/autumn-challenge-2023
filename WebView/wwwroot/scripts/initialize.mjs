import GameClient from "./game-client.mjs";
import DomClient from "./dom-client.mjs";

window.conway = window.conway || {};

/**
 * @param { string } containerId
 * @param { string } fetchPath
 * @param { number } turnSpeedInMs
 * @param { number } turn
 * @param { 'binary'|'color' } color
 * @param { CatalogEntry } optionalSeedOverride
 * @returns { Promise<Game> }
 */
const initialize = async (containerId, fetchPath, turnSpeedInMs, turn, color, optionalSeedOverride) => {
    
    const backendClient = new GameClient(fetchPath);
    const catalog = await backendClient.getCatalog();
    
    const initialSeed = optionalSeedOverride || catalog.filter(x => x.key === "Blinker")[0];
    const initialState = { turn: turn, grid: initialSeed.value };

    const domClient = new DomClient(initialSeed, catalog, color).renderTo(containerId);
    const game = await conway.gameFactory(backendClient, turnSpeedInMs, initialState); // TODO: start game method separate from ctor

    domClient.subscriptions.onCellClick = (async (i, j) =>
        await game
            .pause()
            .seed(i, j)
            .then(async newSeed => await backendClient.fetchStates(newSeed))
            .then(game.setState));

    domClient.subscriptions.onResetBtnClick = () => game
        .pause()
        .reset();

    domClient.subscriptions.onTogglePlayBtnClick = game
        .togglePause;

    domClient.subscriptions.onCatalogSelect = async catalogIndex => {
        const selected = catalog[catalogIndex];
        const params = new URLSearchParams(location.search);
        params.set("color", domClient.getColor());
        params.set("turn-speed", turnSpeedInMs + "");
        params.set("seed", encodeURIComponent(JSON.stringify(selected)));
        window.location.search = params.toString();
    };

    game.subscribe.toChanged(domClient.rerender);

    await game.start();

    return game;
};

export default initialize;