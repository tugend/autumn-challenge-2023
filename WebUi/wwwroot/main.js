window.conway = window.conway || {};

const onPauseBtnClick = () => 
    window.conway.game.togglePause()

const onResetBtnClick = async () => 
    window.conway.game.reset(); 

const onCellClick = async (i, j) => {
    window.conway.game.pause()
    await seed(i, j);
}

const seed = async (i, j) => {
    let newSeed = await conway.game.seed(i, j) 
    let newStates = await window.conway.backendClient.fetchStates(newSeed)
    newStates.forEach(x => x.turn = window.conway.game.index() + x.turn);
    window.conway.game = window.conway.gameFactory(newStates,  window.conway.domClient.render)
    console.log('cell click!', window.conway.game.current(), window.conway.game.isPaused())
    window.conway.domClient.render(window.conway.game.current(), window.conway.game.isPaused());
}

const main = async () => {
    const initialSeed = {
        turn: 0,
        grid: [ [1, 1, 0], [1, 0, 0], [0, 0, 0] ]
    };

    const states = await window.conway.backendClient.fetchStates(initialSeed);
    window.conway.game = window.conway.gameFactory(states, window.conway.domClient.render)
    this.conway.game.unpause();
};

_ = main()



// TODO: cleanup    : cqrs + hide unused from outside, put js files in folder
// TODO: feature    : long running games by automated paging in slices of 1o states
// TODO: feature    : bigger grids!
// TODO: feature    : color coding of cell generations
// TODO: feature    : kill or birth illustration for mouse cell clicks
// TODO: add        : linting
// TODO: consider   : classes instead