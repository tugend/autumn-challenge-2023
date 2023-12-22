window.conway = window.conway || {};



/**
 * @param { string } containerId
 * @param { string } fetchPath
 * @param { number } turnSpeedInMs
 * @param { CatalogEntry } optionalSeedOverride
 * @returns { Promise<Game> }
 */
window.conway.initialize = async (containerId, fetchPath, turnSpeedInMs, optionalSeedOverride) => {
    
    const backendClient = conway.backendClientFactory(fetchPath);
    const catalog = await backendClient.getCatalog();
    
    const initialSeed = optionalSeedOverride || catalog[2];
    const initialState = { turn: 0, grid: initialSeed.value };
    const initialStates = await backendClient.fetchStates(initialState)
    
    const domClient = conway.domClientFactory(initialSeed, catalog).renderTo(containerId);
    let game = conway.gameFactory(turnSpeedInMs, initialStates);

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
    
    // TODO: use game set state and add game setIntiallState?
    domClient.subscribe.toCatalogSelect(async catalogIndex => {
        game.pause();
        
        const selected = catalog[catalogIndex];
        game = await window.conway.initialize(containerId, fetchPath, turnSpeedInMs, selected);
        
        game.unpause();
    });

    game.subscribe.toChanged(domClient.rerender);
    
    game.subscribe.toNextStatePage(async fromState => {
        return await backendClient.fetchStates(fromState);
    })
    
    return game;
}