## TODOs

### Pending refinement
- Use module systems to reduce complexity of web view: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules
  - Cleanup and rename initialize (and maybe refactor to class?)
  - Move index script to a main script (together with initialize maybe?)
  - Cleanup and refactor game-view to class
- Merge to main
- Simplify github actions steps (multiple runs in one step)
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