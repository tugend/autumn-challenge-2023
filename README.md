## TODOs

### Pending refinement
- TODO: reconsider type names (client, manager, game, handler, controller wtf is it....) -> game-api, dom-manager, location-manager, game-controller maybe?   
- TODO: revisit accessors
- split into file per type
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