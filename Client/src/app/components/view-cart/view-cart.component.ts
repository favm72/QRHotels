import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { CartService } from "src/app/services/cart.service"
import { faTrash } from "@fortawesome/free-solid-svg-icons"

@Component({
	selector: "app-view-cart",
	templateUrl: "./view-cart.component.html",
	styleUrls: ["./view-cart.component.css"],
})
export class ViewCartComponent implements OnInit {
	order = null
	categories = []
	removeIcon = faTrash
	comment = ""
	discount = null

	constructor(private cart: CartService, private router: Router) {}

	async ngOnInit(): Promise<void> {
		await this.cart.setOrder(() => {
			this.goBack()
		})
		this.order = this.cart.order
		this.comment = this.cart.order.comment
		this.groupOrder()
	}

	groupOrder() {
		let catids = []
		this.categories = []
		for (const item of this.order.products) {
			let idtoadd = item.idcat != null ? item.idcat : -1
			let nametoadd = idtoadd == -1 ? "Sin Categoría" : item.catname
			if (!catids.includes(idtoadd)) {
				catids.push(idtoadd)
				this.categories.push({
					id: idtoadd,
					name: nametoadd,
					products: this.order.products.filter(x => x.idcat == item.idcat),
				})
			}
		}
	}

	remove(product) {
		this.cart.removeFromCart(product, () => {
			this.goBack()
		})
		this.cart.setOrder(() => {
			this.goBack()
		})
		this.order = this.cart.order
		this.groupOrder()
	}

	goBack() {
		this.router.navigate(["/cart"])
	}

	continue() {
		this.cart.order.comment = this.comment
		this.router.navigate(["/order-confirm"])
	}
}
