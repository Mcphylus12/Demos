import { api } from "../App/Api"
import { Category } from "./Category"

const categoryApi = api.injectEndpoints({
  endpoints: (build) => ({
    getCategories: build.query<Category[], void>({
      query: () => '/api/categories',
    }),
    createCategory: build.mutation<void, Category>({
        query: category => ({
            url: 'api/categories',
            method: 'POST',
            body: category
        })
    })
  })
})

export const { useCreateCategoryMutation, useGetCategoriesQuery } = categoryApi