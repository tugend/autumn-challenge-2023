window.conway = window.conway || {};

window.conway.controllerFactory = ((backendClientFactory, domClientFactory, gameFactory) => {
    let game = null;
    let backendClient = null;
    let domClient = null;
    let initialSeed = null;
    
    const onCellClick = async (i, j) => {
        game.pause()
        
        const newSeed = await game.seed(i, j)
        const newStates = await backendClient.fetchStates(newSeed)
        newStates.forEach(x => x.turn = game.current().turn + x.turn);

        game = gameFactory(newStates, domClient.render)
        game.init();
    }

    const onPauseBtnClick = () =>
        game.togglePause()

    const onResetBtnClick = async () => {
        game.pause()

        const states = await backendClient.fetchStates(initialSeed);
        game = gameFactory(states, domClient.render)
        game.init();
        return game;
    }
    
    const start = async (containerId, seed) => {
        initialSeed = seed;
        backendClient = backendClientFactory("/api/conway");
        domClient = domClientFactory(onCellClick, onPauseBtnClick, onResetBtnClick);
        
        domClient.init(containerId)
        const states = await backendClient.fetchStates(seed);
        game = gameFactory(states, domClient.render)
        game.init();
        game.unpause();
        return game;
    }
    
    return { start }
});

// TODO: cleanup    : Add/fix/update JSDOCs
// TODO: tests      : Add test coverage
// TODO: tag        : version 1.0.0

// TODO: refactor   : replace function factories with classes?
// TODO: feature    : long running games by automated paging in slices of 1o states
// TODO: feature    : bigger grids!
// TODO: feature    : color coding of cell generations
// TODO: feature    : kill or birth illustration for mouse cell clicks
// TODO: add        : linting
// TODO: consider   : classes instead