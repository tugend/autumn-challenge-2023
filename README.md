## TODOs

### Pending refinement
- Simplify github actions steps (multiple runs in one step)
- Add caching to github actions
- Add annotations to github actions
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