import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { CartService } from "src/app/services/cart.service"

@Component({
	selector: "app-addtocart",
	templateUrl: "./addtocart.component.html",
	styleUrls: ["./addtocart.component.css"],
})
export class AddtocartComponent implements OnInit {
	product = null
	modifiers = []
	provider = null
	invalidModifier = false

	constructor(private router: Router, private cart: CartService) {
		if (cart.product == null) {
			this.goBack()
			return
		}
		this.product = cart.product
		this.provider = cart.provider

		if (this.product.quantity == null) {
			this.product.quantity = 1
		}
		if (this.product.comment == null) {
			this.product.comment = ""
		}
		if (this.product.modifiers != null) {
			this.modifiers = JSON.parse(this.product.modifiers)
		}
		this.updateTotalPrice()
	}

	goBack() {
		this.router.navigate(["/cart"])
	}

	updateTotalPrice() {
		this.product.totalPrice = this.product.quantity * this.product.price
	}

	validateModifiers() {
		let invalid = false
		for (const m of this.modifiers) {
			if (m.type == "radio") {
				m.invalid = !m.value
				m.message = `${m.title}: seleccione una opción.`
				invalid = invalid || !m.value
			}
			if (m.type == "checkbox") {
				let count = m.items.filter(x => x.value == true).length
				if (count < m.min) {
					m.invalid = true
					m.message = `${m.title}: seleccione mínimo ${m.min} opciones.`
					invalid = true
				} else if (count > m.max) {
					m.invalid = true
					m.message = `${m.title}: seleccione máximo ${m.max} opciones.`
					invalid = true
				} else {
					m.invalid = false
					m.message = ""
					invalid = invalid || false
				}
			}
		}
		return !invalid
	}

	addToCart() {
		if (!this.validateModifiers()) return
		this.cart.product.modifiers = JSON.stringify(this.modifiers)
		this.cart.product.quantity = this.product.quantity
		this.cart.product.totalPrice = this.product.totalPrice
		this.cart.product.comment = this.product.comment
		if (this.provider == 4)
			this.cart.addToCartBreakFast(() => {
				this.goBack()
			})
		else
			this.cart.addToCart(() => {
				this.goBack()
			})
	}

	plus() {
		this.product.quantity++
		this.updateTotalPrice()
	}

	minus() {
		this.product.quantity = Math.max(this.product.quantity - 1, 1)
		this.updateTotalPrice()
	}

	async ngOnInit(): Promise<void> {}
}
