## TODOs

### Refactor to solve complexity of solution

Refactor to use the new module system, maybe with mjs extensions, neat!
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules

Reconsider JSDOC, classes and typescript after that.

### TODO: fix dangling processes

Introduce a web factory fixture, try catch some more, 
figure out why processes does not shut down correctly

### Pending refinement
* TODO: feature    : long running games by automated paging in slices of 1o states
* TODO: feature    : bigger grids!
* TODO: feature    : color coding of cell generations
* TODO: feature    : kill or birth illustration for mouse cell clicks
* TODO: add        : linting


## Working Notes Tips

### Kill dangling port hugging process
```cmd
netstat -ano | findstr :<PORT>
taskkill /PID <PID> /F

npx kill-port 8080
```
