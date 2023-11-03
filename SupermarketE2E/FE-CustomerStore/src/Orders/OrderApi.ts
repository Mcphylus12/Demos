import { api } from "../App/Api"
import { Order } from "./Order"

const orderApi = api.injectEndpoints({
  endpoints: (build) => ({
    createOrder: build.mutation<void, Order>({
        query: order => ({
            url: 'api/orders',
            method: 'POST',
            body: order
        })
    })
  })
})

export const { useCreateOrderMutation } = orderApi