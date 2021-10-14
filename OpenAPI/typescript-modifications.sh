#!/bin/bash

EXISTS=$(grep "export class ResponseError " huokanclient-ts/src/runtime.ts)

if [[ -z $EXISTS ]]; then
    sed -i -e "s/throw response;/throw new ResponseError(response, 'Response returned an error code');/g" huokanclient-ts/src/runtime.ts
    cat << EndOfCode >> huokanclient-ts/src/runtime.ts

export class ResponseError extends Error {
    constructor(public response: Response, msg?: string) {
        super(msg);
    }
}
EndOfCode
    echo "Added ResponseError."
else
    echo "ResponseError already exists so no changes were made."
fi
