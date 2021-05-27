// JavaScript source code

let error = true

let res = [
    db.container.drop()
]

printjson(res)

if (error) {
    print('Error, exiting')
    quit(1)
}
