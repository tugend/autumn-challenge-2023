const transformSeedToState = (seed) => (
    { 
        turn: 0, 
        grid: seed.value.split(/\r?\n/).map(row => row.split(" ")) 
    });

window.conway = window.conway || {};

window.conway.initialize = async (containerId, fetchPath, turnSpeedInMs, optionalSeedOverride) => {
    
    const backendClient = conway.backendClientFactory(fetchPath);
    const catalog = await backendClient.getCatalog();
    
    const initialSeed = optionalSeedOverride || catalog[2]; // TODO: fix these types
    const initialState = transformSeedToState(initialSeed);
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

    game.subscribe.toChanged(domClient.rerender);
    
    domClient.subscribe.toCatalogSelect(async catalogIndex => {
        console.log('catalog select', catalogIndex);
        game.pause();
        
        // TODO: HACK: LEAVES A DANGLING STATE AT INDEX
        const selected = catalog[catalogIndex];
        game = await window.conway.initialize(containerId, fetchPath, turnSpeedInMs, selected);
        game.unpause();
    });

    return game;
}