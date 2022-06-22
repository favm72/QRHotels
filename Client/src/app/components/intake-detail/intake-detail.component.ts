import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { IntakeService } from "src/app/services/intake.service"

@Component({
	selector: "app-intake-detail",
	templateUrl: "./intake-detail.component.html",
	styleUrls: ["./intake-detail.component.css"],
})
export class IntakeDetailComponent implements OnInit {
	detailList = []
	currentIntake = null
	hotelCode = ""
	constructor(private intakeSrv: IntakeService, private router: Router) {}

	async ngOnInit(): Promise<void> {
		if (this.intakeSrv.currentIntake == null) {
			this.router.navigate(["/intake"])
			return
		}
		this.currentIntake = this.intakeSrv.currentIntake
		const intakeData = await this.intakeSrv.loadIntakeDetail()
		if (intakeData) {
			this.detailList = intakeData.detail
			this.hotelCode = intakeData.hotelCode
		}
	}

	goBack() {
		this.router.navigate(["/intake"])
	}
	get currency() {
		return this.currentIntake.currency == "SOL" ? "S/." : this.currentIntake.currency
	}
	get totals() {
		const result = this.detailList.reduce(
			(tot, item) => {
				tot.discount += item.discount
				tot.surcharge += item.surcharge
				tot.amount += item.amount
				return tot
			},
			{
				discount: 0,
				surcharge: 0,
				amount: 0,
				base: 0,
				tax: 0,
				service: 0,
			}
		)
		result.base = (result.amount - result.discount) / (this.hotelCode === "PUCAL" ? 1.05 : 1.28)
		result.tax = result.base * 0.18
		result.service = result.base * 0.1
		return result
	}
}
