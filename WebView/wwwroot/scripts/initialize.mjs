import GameClient from "./game-client.mjs";
import DomClient from "./dom-client.mjs";
import GameView from "./game-view.mjs";

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

    const domClient = new DomClient(initialSeed, catalog, color);

    const game = await GameView(backendClient.fetchStates, turnSpeedInMs, initialState); // TODO: start game method separate from ctor

    domClient.subscribe.toCellClick(((i, j) =>
        game
            .pause()
            .seed(i, j)
            .then(async newSeed => await backendClient.fetchStates(newSeed))
            .then(game.setState)));

    domClient.subscribe.toResetBtnClick(() => game
        .pause()
        .reset());

    domClient.subscribe.toTogglePlayBtnClick(game
        .togglePause);

    domClient.subscribe.toCatalogSelect(catalogIndex => {
        const selected = catalog[catalogIndex];
        const params = new URLSearchParams(location.search);
        params.set("color", domClient.getColor());
        params.set("turn-speed", turnSpeedInMs + "");
        params.set("seed", encodeURIComponent(JSON.stringify(selected)));
        window.location.search = params.toString();
    });

    game.subscribe.toChanged(domClient.rerender);

    domClient.renderTo(containerId);
    await game.start();

    return game;
};

export default initialize;