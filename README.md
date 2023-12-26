﻿## TODOs

### Pending refinement
- Fix fetchStates injection in game-factory.js
- Reference dlls in test startup fixtures
- Cleanup stringify vs convert
- use module systems to reduce complexity of web view: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules
- TODO: clean up notes and replace with a README file
- Try to simplify github actions steps (multiple runs in one step)

## Working Notes Tips

### Kill dangling port hugging process
```cmd
netstat -ano | findstr :<PORT>
taskkill /F /PID <PID>

npx kill-port 8080
```

Windows -> open resource manager, click cpu then search for the locked file
Windows task manager -> WebUi -> close