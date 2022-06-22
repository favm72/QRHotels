import { Component, ElementRef, OnInit, ViewChild } from "@angular/core"
import { Router } from "@angular/router"
import { CartService } from "src/app/services/cart.service"
import { InfoService } from "src/app/services/info.service"

@Component({
	selector: "app-sendorder",
	templateUrl: "./sendorder.component.html",
	styleUrls: ["./sendorder.component.css"],
})
export class SendorderComponent implements OnInit {
	form = {
		prefix: { value: "51", invalid: false },
		number: { value: "", invalid: false },
		schedule: { value: 0, invalid: false },
		breakfastdate: { value: "", invalid: false, min: "" },
		payment: { value: 0, invalid: false },
	}

	esDesayunoHome: boolean = false
	montoDesayuno: number = 0
	order = null
	provider = null
	schedule = []
	paymtypes = []

	@ViewChild("mDate") mDate: ElementRef
	@ViewChild("mPrefix") mPrefix: ElementRef
	@ViewChild("mNumber") mNumber: ElementRef
	@ViewChild("mSchedule") mSchedule: ElementRef

	formatDate(date) {
		var d = new Date(date),
			month = "" + (d.getMonth() + 1),
			day = "" + d.getDate(),
			year = d.getFullYear()

		if (month.length < 2) month = "0" + month
		if (day.length < 2) day = "0" + day

		return [year, month, day].join("-")
	}

	constructor(private router: Router, private cart: CartService, private info: InfoService) {
		this.provider = cart.provider
		this.order = cart.order

		if (localStorage.number != null) this.form.number.value = localStorage.number
		if (localStorage.prefix != null) this.form.prefix.value = localStorage.prefix

		if (cart.provider == 4) {
			this.form.breakfastdate.min = this.formatDate(new Date())
			let tomorrow = new Date()
			tomorrow.setDate(new Date().getDate() + 1)
			this.form.breakfastdate.value = this.formatDate(tomorrow)
		}
	}

	validateDate() {
		this.form.breakfastdate.invalid = this.form.breakfastdate.value == ""
	}
	validatePrefix() {
		this.form.prefix.invalid = !/^[0-9]{1,4}$/.test(this.form.prefix.value)
	}
	validateNumber() {
		this.form.number.invalid = !/^[0-9]{9}$/.test(this.form.number.value)
	}
	validateSchedule() {
		this.form.schedule.invalid = this.form.schedule.value < 1
	}
	validatePayment() {
		this.form.payment.invalid = this.form.payment.value < 1
	}

	validateForm(): boolean {
		this.validatePrefix()
		this.validateNumber()
		this.validatePayment()

		if (this.form.prefix.invalid) {
			this.mPrefix.nativeElement.focus()
			return false
		}
		if (this.form.number.invalid) {
			this.mNumber.nativeElement.focus()
			return false
		}
		if (this.form.payment.invalid) return false
		if (this.provider == 4) {
			this.validateDate()
			this.validateSchedule()

			if (this.form.breakfastdate.invalid) {
				this.mDate.nativeElement.focus()
				return false
			}
			if (this.form.schedule.invalid) {
				this.mSchedule.nativeElement.focus()
				return false
			}
		}
		return true
	}

	async saveOrder() {
		if (!this.validateForm()) return
		this.cart.order.prefix = this.form.prefix.value
		this.cart.order.number = this.form.number.value
		this.cart.order.idPaymType = this.form.payment.value
		if (this.cart.provider == 4) {
			this.cart.order.breakFastDateString = this.form.breakfastdate.value
			this.cart.order.idSchedule = this.form.schedule.value
		}
		await this.cart.saveOrder(() => {
			this.router.navigate(["/master/main"])
		})
	}

	async ngOnInit(): Promise<void> {
		this.paymtypes = await this.info.paymtypes()
		if (this.cart.provider == 4) {
			this.schedule = await this.info.loadSchedule()
			let user = JSON.parse(localStorage.qrh)
			if (user.tipo == "HOME") {
				this.esDesayunoHome = true
				this.montoDesayuno = 25 * user.adults
			}
		}
	}

	goBack() {
		this.router.navigate(["/order"])
	}
}
