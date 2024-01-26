## TODOs

### Refactor to solve complexity of solution

Refactor to use the new module system, maybe with mjs extensions, neat!
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules

Reconsider JSDOC, classes and typescript after that.

### TODO: fix dangling processes

Introduce a web factory fixture, try catch some more, 
figure out why processes does not shut down correctly

### Pending refinement
- Cleanup stringify vs convert
- Split extensions up consistently in partial files
- Split tests up into separate projects
- Align file and solution folder structure to avoid confusion
- {src/test}/{domain,cli,web,extensions}
- Fix dotnet folder structure: domain/{src, test}, cli/{src,test}, web/{src,test}, extensions/?
- Try to simplify github actions steps (multiple runs in one step)
- TODO: clean up notes and replace with a README file

## Working Notes Tips

### Kill dangling port hugging process
```cmd
netstat -ano | findstr :<PORT>
taskkill /F /PID <PID>

npx kill-port 8080
```

Windows -> open resource manager, click cpu then search for the locked file
Windows task manager -> WebUi -> close