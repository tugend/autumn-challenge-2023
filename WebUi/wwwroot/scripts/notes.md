## TODOs

### Refactor to solve complexity of solution

Refactor to use the new module system, maybe with mjs extensions, neat!
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules

Reconsider JSDOC, classes and typescript after that.

### TODO: fix dangling processes

Introduce a web factory fixture, try catch some more, 
figure out why processes does not shut down correctly

### Pending refinement
- TODO: add        : linting
- Add support for full state setting control via url
- Preserve theme settings (color, speed, selection) on catalog select

## Working Notes Tips

### Kill dangling port hugging process
```cmd
netstat -ano | findstr :<PORT>
taskkill /F /PID <PID>

npx kill-port 8080
```

Windows -> open resource manager, click cpu then search for the locked file
Windows task manager -> WebUi -> close