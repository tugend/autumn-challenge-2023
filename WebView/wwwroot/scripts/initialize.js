﻿window.conway = window.conway || {};

/**
 * @param { string } containerId
 * @param { string } fetchPath
 * @param { number } turnSpeedInMs
 * @param { number } turn
 * @param { 'binary'|'color' } color
 * @param { CatalogEntry } optionalSeedOverride
 * @returns { Promise<Game> }
 */
window.conway.initialize = async (containerId, fetchPath, turnSpeedInMs, turn, color, optionalSeedOverride) => {
    
    const backendClient = conway.backendClientFactory(fetchPath);
    const catalog = await backendClient.getCatalog();
    
    const initialSeed = optionalSeedOverride || catalog.filter(x => x.key === "Blinker")[0];
    const initialState = { turn: turn, grid: initialSeed.value };

    const domClient = conway.domClientFactory(initialSeed, catalog, color).renderTo(containerId);
    const game = await conway.gameFactory(backendClient.fetchStates, turnSpeedInMs, initialState); // TODO: start game method separate from ctor

    domClient.subscribe.toCellClick(async (i, j) =>
        await game
            .pause()
            .seed(i, j)
            .then(async newSeed => await backendClient.fetchStates(newSeed))
            .then(game.setState));

    domClient.subscribe.toResetBtnClick(() => game
        .pause()
        .reset());

    domClient.subscribe.toTogglePlayBtnClick(game
        .togglePause);
    
    domClient.subscribe.toCatalogSelect(async catalogIndex => {
        const selected = catalog[catalogIndex];
        const params = new URLSearchParams(location.search);
        params.set("color", domClient.getColor());
        params.set("turn-speed", turnSpeedInMs + "");
        params.set("seed", encodeURIComponent(JSON.stringify(selected)));
        window.location.search = params.toString();
    });

    game.subscribe.toChanged(domClient.rerender);

    await game.start();

    return game;
};