window.conway = window.conway || {};

window.conway.initialize = async (containerId, fetchPath, turnSpeedInMs, initialSeed) => {
    const backendClient = conway.backendClientFactory(fetchPath);
    const initialStates = await backendClient.fetchStates(initialSeed)
    const catalog = await backendClient.getCatalog();
    
    const domClient = conway.domClientFactory(catalog).renderTo(containerId);
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
    
    domClient.subscribe.toCatalogSelect(async newSeed => {
        console.log('catalog select', newSeed);
        game.pause();
        // TODO: HACK: LEAVES A DANGLING STATE AT INDEX
        const newNewSeed = { turn: 0, grid: newSeed.split(/\r?\n/).map(row => row.split(" ")) };

        game = await window.conway.initialize(containerId, fetchPath, turnSpeedInMs, newNewSeed);
        document.querySelector("#conway #state").style.gridTemplateColumns = "1fr ".repeat(newNewSeed.grid.length);

        game.unpause();
    });

    return game;
}