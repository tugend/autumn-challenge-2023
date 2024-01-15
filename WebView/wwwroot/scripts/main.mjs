import GameClient from "./game-client.mjs";
import DomClient from "./dom-client.mjs";
import GameController from "./game-controller.mjs";

export default class GameFactory {
    /**
     * @param { string } containerId
     * @param { string } fetchPath
     * @param { number } turnSpeedInMs
     * @param { number } turn
     * @param { 'binary'|'color' } color
     * @param { CatalogEntry } optionalSeedOverride
     */
    static async initialize(containerId, fetchPath, turnSpeedInMs, turn, color, optionalSeedOverride) {
        const gameClient = new GameClient(fetchPath);
        const catalog = await gameClient.getCatalog();

        const initialSeed = optionalSeedOverride || catalog.filter(x => x.key === "Blinker")[0];
        const initialState = {turn: turn, grid: initialSeed.value};

        const domClient = new DomClient(initialSeed, catalog, color);
        const controller = await new GameController(gameClient.fetchStates, turnSpeedInMs, initialState);

        domClient.subscribe.toCellClick(async (i, j) => {
            controller.pause();
            const newSeed = await controller.seed(i, j);
            const newStates = await gameClient.fetchStates(newSeed);
            controller.setState(newStates);
        });

        domClient.subscribe.toResetBtnClick(() => {
            controller.pause();
            controller.reset();
        });

        domClient.subscribe.toTogglePlayBtnClick(controller
            .togglePause);

        domClient.subscribe.toCatalogSelect(catalogIndex => {
            const selected = catalog[catalogIndex];
            const params = new URLSearchParams(location.search);
            params.set("color", domClient.getColor());
            params.set("turn-speed", turnSpeedInMs + "");
            params.set("seed", encodeURIComponent(JSON.stringify(selected)));
            window.location.search = params.toString();
        });

        controller.subscribe.toChanged(domClient.rerender);

        domClient.renderTo(containerId);
        await controller.start();

        return controller;
    };
}