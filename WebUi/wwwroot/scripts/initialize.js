window.conway = window.conway || {};

window.conway.initialize = async (containerId, fetchPath, initialSeed) => {
    const backendClient = conway.backendClientFactory(fetchPath);
    const initialStates = await backendClient.fetchStates(initialSeed)

    const domClient = conway.domClientFactory().renderTo(containerId);
    const game = conway.gameFactory(initialStates);

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
    
    return game;
}