window.conway = window.conway || {};

window.conway.controllerFactory = ((backendClient, domClient, gameFactory, seed) => {
    let game = null;
    
    const onCellClick = async (i, j) => {
        game.pause()
        
        const newSeed = await game.seed(i, j)
        const newStates = await backendClient.fetchStates(newSeed)
        newStates.forEach(x => x.turn = game.current().turn + x.turn);

        game = conway.gameFactory(newStates, domClient.render)
        game.init();
    }

    const onPauseBtnClick = () =>
        game.togglePause()

    const onResetBtnClick = async () =>
        game.reset();
    
    const start = async () => {
        const states = await window.conway.backendClient.fetchStates(seed);
        game = conway.gameFactory(states, domClient.render)
        game.init();
        game.unpause();
        return game;
    }
    
    return {
        start,
        onCellClick,
        onPauseBtnClick,
        onResetBtnClick
    }
});



// TODO: cleanup    : hide unused from outside, put js files in folder, jsdocs
// TODO: feature    : long running games by automated paging in slices of 1o states
// TODO: feature    : bigger grids!
// TODO: feature    : color coding of cell generations
// TODO: feature    : kill or birth illustration for mouse cell clicks
// TODO: add        : linting
// TODO: consider   : classes instead