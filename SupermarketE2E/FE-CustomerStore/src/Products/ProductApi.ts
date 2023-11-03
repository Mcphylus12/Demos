import { api } from "../App/Api"
import { Product } from "./Product"

const productApi = api.injectEndpoints({
  endpoints: (build) => ({
    getProducts: build.query<Product[], string | undefined>({
      query: category => `/api/products?category=${category}`,
    }),
    createProduct: build.mutation<void, Product>({
        query: item => ({
            url: 'api/products',
            method: 'POST',
            body: item
        })
    })
  })
})

export const { useGetProductsQuery, useCreateProductMutation } = productApi