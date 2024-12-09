import xior from 'xior'

const xiorInstance = xior.create({
    baseURL: "/api"    
})

export const submitFiles = (payload: FormData) => {
    return xiorInstance.post(
        '/document/upload',
        payload
    )
}


export const downloadContract = (contractId: string) => {
    return xiorInstance.get(`/document/downloadcontract/${contractId}`)
}